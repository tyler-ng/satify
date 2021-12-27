import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import moment from "moment";
Vue.config.productionTip = false;
Vue.filter("money", function (number) {
  if (isNaN(number)) {
    return "-";
  }
  return "$ " + Number(number).toFixed(2);
});
Vue.filter("humanizeDate", function (date) {
  return moment(date).format("MMMM Do YYYY");
});
new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
//# sourceMappingURL=main.js.map
