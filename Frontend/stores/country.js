import { defineStore } from "pinia"

export const useCountryStore = defineStore({
    id: 'country-store',
    state: () => {
        return {
            loading: false,
            countriesData: []
        }
    },
    actions: {
        loadCountries(){
            this.loading = true;

            useWebApiFetch('/Country/GetCountries')
                .then(({data, error}) => {
                    if(data.value){
                        this.countriesData = data.value.countries;
                    } else if(error.value){
                        this.countriesData = [];
                    }
                }).finally(() => {
                    this.loading = false;
                });
        },
    }
})