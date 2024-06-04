<template>
  <v-app id="inspire">
    <v-app-bar>
      <v-app-bar-nav-icon v-if="mobile" @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-app-bar-title>FootballHub</v-app-bar-title>
    </v-app-bar>

    <v-navigation-drawer :order="mobile ? -1 : 0" v-model="drawer">
      <!--  -->
    </v-navigation-drawer>

    <v-main>
      <div class="pa-4">
        <NuxtPage />
      </div>
    </v-main>
  </v-app>
</template>

<script setup>
  import { useDisplay } from "vuetify";

  const { mobile } = useDisplay();

  const drawer = ref(false)
</script>