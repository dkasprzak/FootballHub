import vuetify, { transformAssetUrls } from 'vite-plugin-vuetify'
export default defineNuxtConfig({
  runtimeConfig:{
    public:{
      BASE_URL: 'http://localhost:5272',
    },
  },
  devtools: { enabled: false },
  ssr: false,
  build: {
    transpile: ['vuetify'],
  },

  imports: {
    dirs: ['stores']
  },

  modules: [
    (_options, nuxt) => {
      nuxt.hooks.hook('vite:extendConfig', (config) => {
        // @ts-expect-error
        config.plugins.push(vuetify({ autoImport: true }))
      })
    },
    '@pinia/nuxt',
    '@vueuse/nuxt',
    'nuxt-lodash',
    'dayjs-nuxt'
  ],
  vite: {
    vue: {
      template: {
        transformAssetUrls,
      },
    },
  },

  lodash: {
    prefix: "_",
    prefixSkip: false,
    upperAfterPrefix: false,
  }
})