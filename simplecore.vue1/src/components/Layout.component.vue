<template>
    <section>
        <header>
            <menu>
                <router-link to="/">Home</router-link>
                <router-link to="/About">About</router-link>
            </menu>
        </header>
        <div>
            <router-view v-slot="{ Component }">
                <keep-alive>
                    <transition name="fade" mode="out-in">
                        <component :is="Component" />
                    </transition>
                </keep-alive>
            </router-view>
        </div>
        <footer>footer</footer>
    </section>
</template>