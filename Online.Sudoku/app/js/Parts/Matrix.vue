<template>
  <div class="matrix-container">
    <div>
      <table id="matrix" class="table table-bordered">
        <tr v-bind:key="rowIndex" v-for="(row, rowIndex) in model">
          <td
            v-bind:key="cellIndex"
            v-for="(cell, cellIndex) in row"
            v-bind:class="areaHighlight(rowIndex,cellIndex)"
          >
            <template v-if="cell==null">
              <input
                class="cell"
                v-bind:class="cellClass(rowIndex,cellIndex)"
                v-on:keydown="keydown($event,rowIndex,cellIndex)"
                v-model="model[rowIndex][cellIndex]"
              >
            </template>
            <template v-else>
              <input
                class="cell"
                disabled
                v-model="model[rowIndex][cellIndex]"
                v-bind:class="cellClass(rowIndex,cellIndex)"
              >
            </template>
          </td>
        </tr>
      </table>
    </div>

    <div>
      <div></div>

      <div class="list-group">
        <a href="#" class="list-group-item list-group-item-action active">Сейчас в игре:</a>
        <a
          href="#"
          class="list-group-item list-group-item-action"
          v-bind:key="index"
          v-for="(user, index) in users"
        >{{user.Name}}</a>
      </div>
      <div>
        <div id="error" v-if="moveError!=null" class="alert alert-danger">
          <strong>Внимание!</strong>
          {{moveError}}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import postData from "./../Plugins/request";
import { setTimeout } from "timers";
import { loadavg } from "os";

export default {
  props: { hub: Object, user: String, users: Array, model: Array },

  data: function() {
    return {
      errorRow: null,
      errorColumn: null,
      moveError: null
    };
  },
  computed: {},
  mounted: function() {},

  methods: {
    areaHighlight(row, cell) {
      var result = [];

      if ((cell + 1) % 3 == 0) {
        result.push("area-column-right");
      }

      if ((row + 1) % 3 == 0) {
        result.push("area-column-bottom");
      }
      return result;
    },

    cellClass: function(row, cell) {
      var result = [];
      if (this.errorRow == row && this.errorColumn == cell) {
        result.push("error-cell");
      }
      return result;
    },

    keydown: function(e, row, cell) {
      if (e.key.search(/[1-9]/i) == -1) {
        e.preventDefault();
        return;
      }

      let cellModel = {
        Column: cell,
        Row: row,
        Value: e.key
      };

      this.hub.server.insertCell(cellModel);

      e.preventDefault();
      return;
    },

    move: function(response) {
      if (response.Status == "NoSolution") {
        this.moveError = response.Error;
        setTimeout(() => {
          this.moveError = null;
        }, 1000);
      } else if (response.Status == "Error") {
        this.errorRow = response.Cell.Row;
        this.errorColumn = response.Cell.Column;
        this.moveError = response.Error;
        setTimeout(() => {
          this.errorRow = null;
          this.errorColumn = null;
          this.moveError = null;
        }, 500);
      } else {
        this.model[response.Cell.Row][response.Cell.Column] =
          response.Cell.Value;

        Vue.set(this.model, response.Cell.Row, this.model[response.Cell.Row]);

        console.log(response);
        if (response.Status == "Win") {
          this.$emit("win", response.Results);
        }

        if (response.Status == "Lost") {
          this.$emit("lost");
        }
      }
    },

    loading: function() {
      this.$emit("update:loading", true);
    }
  }
};
</script>

<style scoped>
#matrix {
  margin-left: auto;
  margin-right: auto;
  border: 2px solid grey;
}
.area-column-right {
  border-right: 2px solid grey;
}

.area-column-bottom {
  border-bottom: 2px solid grey;
}

.matrix-container {
  margin-top: 5px;
  width: 100%;
  display: flex;
  justify-content: center;
}
.list-group,
#error {
  margin-left: 15px;
  width: 150px;
}

.cell {
  width: 60px;

  height: 60px;
}

.error-cell {
  border-color: red;
}
#matrix {
  width: 500px;
}

#matrix tr {
  height: 25px;
}

#matrix td {
  width: 25px;
  text-align: center;
}
</style>