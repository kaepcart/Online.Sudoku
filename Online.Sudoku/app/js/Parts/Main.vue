
<template >
  <div>
    <div class="navbar navbar-inverse navbar-fixed-top">
      <div class="container">
        <div class="navbar-header">
          <button
            type="button"
            class="navbar-toggle"
            data-toggle="collapse"
            data-target=".navbar-collapse"
          >
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#" v-on:click="getMainMenu()">Sudoku</a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li>
              <a class="navbar-brand" href="#" v-on:click="newGame()">Новая игра</a>
            </li>
            <li>
              <a class="navbar-brand" href="#" v-on:click="joinGame()">Присоединится к игре</a>
            </li>
            <li>
              <a
                class="navbar-brand"
                v-if="Mode==Modes.matrix"
                href="#"
                v-on:click="endGame()"
              >Закончить игру</a>
            </li>
            <li>
              <a class="navbar-brand" v-on:click="getTop()" href="#">Топ</a>
            </li>
          </ul>
        </div>
      </div>
    </div>

    <div class="loader-container" v-if="loading">
      <div class="loader"></div>
    </div>
    <template v-if="Mode==Modes.matrix">
      <sudoku-matrix
        v-bind:loading.sync="loading"
        ref="matrix"
        v-bind:hub="hub"
        v-bind:user="user"
        v-bind:users="users"
        v-bind:model="model"
        v-on:win="win"
        v-on:lost="lost"
      ></sudoku-matrix>
    </template>

    <template v-if="Mode==Modes.mainMenu">
      <div class="alert alert-warning" v-if="isLost">Вы проиграли попробуйте ещё раз!</div>
      <br>
      <div class="mainMenu">
        <div>Введите имя:</div>
        <div>
          <input v-model="user">
        </div>
      </div>
    </template>

    <template v-if="Mode==Modes.top">
      <top v-bind:topUsers="topUsers"></top>
    </template>

    <template v-if="Mode==Modes.win">
      <win v-bind:results="results" v-bind:hub="hub" v-bind:loading.sync="loading"></win>
    </template>
  </div>
</template>


<script>
import Matrix from "./Matrix.vue";
import Top from "./Top.vue";
import Win from "./Win.vue";
import { setTimeout } from "timers";

export default {
  components: {
    "sudoku-matrix": Matrix,
    top: Top,
    win: Win
  },
  data: () => {
    return {
      user: "",
      top: false,
      loading: false,
      client: null,
      Modes: { mainMenu: 1, matrix: 2, top: 3, win: 4 },
      Mode: 1,
      hub: null,
      users: [],
      model: null,
      topUsers: [],
      results: null,
      isLost: false
    };
  },
  mounted: function() {
    var hub = $.connection.sudokuHub;
    this.hub = hub;

    $.connection.hub.start({ transport: ["webSockets"] }).done(() => {
    });

    $.connection.hub.error(error => {
      this.Mode = Modes.mainMenu;
    });

    hub.client.InsertNumber = obj => {
      this.$refs.matrix.move(obj);
    };

    hub.client.RemoveUser = obj => {
      this.users = this.users.filter(x => {
        x.ConnectionId != obj;
      });
    };

    hub.client.AddUser = obj => {
      this.users = obj;
    };

    hub.client.GetMatrix = matrix => {
      console.log(matrix);
      this.loading = false;

      if (matrix.Status == "Error") {
        alert(matrix.Error);
        return;
      }
      this.model = matrix.Field;
      this.Mode = this.Modes.matrix;

      // this.$refs.matrix.model = matrix.Field;
    };

    //Работа с пользователями
    hub.client.GetUserTop = top => {
      this.loading=false;
      this.topUsers = top;
    };

    hub.client.UserAdded = top => {
      this.getTop();
      this.loading = false;
    };
  },

  methods: {
    getMainMenu: function() {
      this.Mode = this.Modes.mainMenu;
      this.isLost = false;
    },

    connect: function() {
      if (this.user.length < 3) {
        alert(
          "Необходимо внести имя для начала игры. Имя должно включать более 3х символов."
        );
        this.getMainMenu();
        return false;
      }
      this.hub.server.connected(this.user);
      return true;
    },

    newGame: function(e) {
      if (!this.connect()) {
        return;
      }

      this.hub.server.newGame();
    },

    endGame: function(e) {
      this.hub.server.endGame();
       this.getMainMenu();
    },

    joinGame: function() {
      if (!this.connect()) {
        return;
      }
      //  this.Mode = this.Modes.matrix;
      this.hub.server.joinGame();
    },

    win: function(results) {
      this.results = results;
      this.Mode = this.Modes.win;
    },

    lost: function() {
      this.Mode = this.Modes.mainMenu;
      this.isLost = true;
    },

    getTop: function() {
      this.loading=true;
      this.hub.server.usersTop();
      this.Mode = this.Modes.top;
    }
  }
};
</script>


<style scoped>
.mainMenu {
  display: grid;
  grid-template-columns: 100px 200px;
  align-items: center;
  justify-content: center;
  margin-top: 20px;
}

.loader-container {
  position: absolute;
  width: 100%;
  height: 100%;
  background: black;
  opacity: 0.4;
}

.loader {
  border: 16px solid green; /* Light grey */
  border-top: 16px solid #3498db; /* Blue */
  border-radius: 50%;
  width: 500px;
  height: 500px;
  animation: spin 2s linear infinite;
  top: 10%; /* position the top  edge of the element at the middle of the parent */
  margin-left: 35%;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>