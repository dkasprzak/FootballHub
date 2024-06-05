<template>
  <v-app id="inspire">
    <v-app-bar color="brand">
      <v-app-bar-nav-icon v-if="mobile" @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-app-bar-title>FootballHub</v-app-bar-title>
      <v-spacer></v-spacer>
      <VBtn @click="toggleTheme" icon="mdi-theme-light-dark" title="Przełącz motyw"></VBtn>
    </v-app-bar>

    <v-navigation-drawer :order="mobile ? -1 : 0" v-model="drawer">
      <VList>
        <VListItem v-for="item in menuItems" :key="item.name" :title="item.name" :prepend-icon="item.icon" :to="item.url"></VListItem>
      </VList>
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
  import { useTheme } from 'vuetify'
  import { useStorage } from '@vueuse/core'


  const theme = useTheme();
  const { mobile } = useDisplay();
  const drawer = ref(null);
  const currentTheme = useStorage('currentTheme', 'light');
  const userStore = useUserStore();

  const menuItems = [
    {
      name: 'Strona główna',
      icon: 'mdi-home',
      url: '/'
    },
    {
      name: 'Kluby',
      icon: 'mdi-soccer',
      url: '/football-clubs'
    }
  ];

function toggleTheme () {
  let newTheme =  theme.global.current.value.dark ? 'light' : 'dark';
  theme.global.name.value = newTheme;
  currentTheme.value = newTheme;
}

//Przypisanie koloru z local storage
onMounted(() => {
  theme.global.name.value = currentTheme.value;
  userStore.loadLogedInUser();
});

</script>