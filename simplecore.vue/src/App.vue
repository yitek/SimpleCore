<template>
  <div id="app">
    Loading...
  </div>
</template>

<script lang="ts">

(window as any).eventBus = new Vue()

</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
