// import {Vue} from "vue";
var Vue = require("vue");

var elementId = "app";

var app = new Vue({
  el: '#'+elementId,
  data: {
    message: "Hello from vue"
  }
})