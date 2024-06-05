import '@mdi/font/css/materialdesignicons.css'
import colors from 'vuetify/util/colors'

import 'vuetify/styles'
import { createVuetify } from 'vuetify'

export default defineNuxtPlugin((app) => {
  const vuetify = createVuetify({
    defaults: {
      global: {
      },
      VList: {
      },
    },
    theme: {
      defaultTheme: 'light',
      themes: {
        light: {
          colors: {
            'brand': colors.green.accent2,
          }
        },
        dark: {
          colors: {
            'brand': colors.green.darken4
          }
        }
      }
    }
  })
  app.vueApp.use(vuetify)
})