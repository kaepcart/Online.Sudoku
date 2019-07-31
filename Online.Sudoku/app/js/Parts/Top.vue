<template >
<div>
    <h4>Топ 50 лучших игр:</h4>
  <table class="table table-striped table-bordered">
    <thead>
      <tr>
        <th>№</th>
        <th>Имя пользователя</th>
        <th>Количество ходов</th>
        <th>Время на игру</th>
      </tr>
    </thead>
    <tbody>
      <tr v-bind:class="rowClass(user)" v-bind:key="index" v-for="(user, index) in topUsers">
        <td>{{index+1}}</td>
        <td>{{user.UserName}}</td>
        <td>{{user.Count}}</td>
        <td>{{user.Time}}</td>
      </tr>
    </tbody>
  </table>
  </div>
</template>


<script>
export default {
  props: { topUsers: Array },
  methods: {
    rowClass: function(user) {
     
       var result = [];
      if (user.IsCurrent) {
        result.push("current-user");
      }
      return result;

    }
  }
};
</script>


<style scoped>
.current-user
{
    background-color: lightgreen!important;
}
</style>