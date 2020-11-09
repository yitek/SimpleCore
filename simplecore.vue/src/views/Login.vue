<template>
  <el-form label-position="left" label-width="100px" :model="data">
    <el-form-item label="用户名">
      <el-input v-model="data.username"></el-input>
    </el-form-item>
    <el-form-item label="密码">
      <el-input type="password" v-model="data.password"></el-input>
    </el-form-item>
    <div>
      <el-button @click="handleLogin">登录</el-button>
    </div>
</el-form>
</template>

<script lang="ts">
import { Component, Emit, Inject, Model, Prop, Provide, Vue, Watch } from 'vue-property-decorator'
@Component
export default class Login extends Vue{
  data= {
        username: 'yiy',
        password: 'yiy'
  }

  handleLogin(){
    let model = { 'username': this.data.username,'password': this.data.password};
    console.log(model);
  }
}
</script>
