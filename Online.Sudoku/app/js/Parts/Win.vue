<template >
  <div style="text-align:center">
    <h4>Поздравляем Вы выиграли, Ваш результат</h4>

    <div class="win-container">
      <table id="result" class="table table-striped table-bordered table-dark">
        <tbody>
          <tr>
            <td>Имя:</td>
            <td>{{results.UserName}}</td>
          </tr>
          <tr>
            <td>Количество ходов:</td>
            <td>{{results.Count}}</td>
          </tr>
          <tr>
            <td>Игровое время:</td>
            <td>{{results.Time}}</td>
          </tr>
        </tbody>
      </table>
      <a id="AddInTop" class="btn btn-sm btn-primary" v-on:click="AddInTop()">Добавить в топ</a>
    </div>
  </div>
</template>


<script>
export default {
  props: { results: Object, hub: Object },
  methods: {
    AddInTop: function() {
       this.$emit("update:loading", true);
      this.hub.server.addUserInTop(this.results);
    }
  }
};
</script>


<style scoped>
#result {
  width: 250px;
}

.win-container {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}

#AddInTop {
  width: 250px !important;
}
</style>