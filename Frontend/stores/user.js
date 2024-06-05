import { defineStore } from "pinia";

export const useUserStore = defineStore({
    id: 'user-store',
    state: () => {
        return {
            isLoggedIn: false,
            loading: false,
            userData: null,
        }
    },
    actions: {
        loadLogedInUser(){
            this.loading = true;
            useWebApiFetch('/User/GetLoggedInUser')
                .then(({ data, error }) => {
                    if(data.value){
                        this.isLoggedIn = true;
                        this.userData = data.value;
                    } else if(error.value){
                        this.isLoggedIn = false;
                        this.userData = null;
                    }
                })
                .finally(() => {
                    this.loading = false;
                });
        },

        logout(){
            useWebApiFetch('/User/Logout', {
                method: 'POST'
            })
                .then((response) => {
                    if(response.data.value){
                        this.isLoggedIn = false;
                        this.userData = null;
                    }
                })
        },
    }
})