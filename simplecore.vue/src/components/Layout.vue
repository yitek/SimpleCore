<template>
    <div>
        <el-container>
            <el-header>Header</el-header>
            <el-container>
                <el-aside width="200px">
                    <menu>
                        <router-link to="/">Home</router-link>
                        <router-link to="/about">About</router-link>
                    </menu>
                </el-aside>
                <el-container>
                    <el-main>
                        <router-view />
                    </el-main>
                </el-container>
            </el-container>
            <el-footer>Footer</el-footer>
        </el-container>
    </div>
</template>
<style scoped></style>
