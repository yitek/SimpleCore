<template>
    <el-table
      :data="data.items"
      style="width: 100%">
      <el-table-column
        prop="date"
        label="日期"
        width="180">
      </el-table-column>
      <el-table-column
        prop="name"
        label="姓名"
        width="180">
      </el-table-column>
      <el-table-column
        prop="address"
        label="地址">
      </el-table-column>
    </el-table>
</template>

<script lang="ts">
import { Component, Emit, Inject, Model, Prop, Provide, Vue, Watch } from 'vue-property-decorator'
import { request,Response } from '@/utils/request'

@Component
export default class UserList extends Vue{
    
    data={
        items:[]
    };

    constructor(){
        super()
        request({
            url:'account/index',
            method:'post'
        }).then((ret: Response) => {
            console.log(ret)
        })
    }
}
</script>
