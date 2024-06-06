<template>
    <VDialog :model-value="show" persistent width="400" scroll-strategy="none">
        <VCard class="py-4">
            <VCardTitle class="text-center">Logowanie</VCardTitle>
            <div v-if="userStore.$state.loading === true" class="pa-4 d-flex justify center">
                <VProgressCircular indeterminate></VProgressCircular>
            </div>
            <VForm v-else @submit.prevent="submit" :disabled="loading">
                <VCardText>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.email" label="Email" :rules="[ruleRequired, ruleEmail]"></v-text-field>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.password" type="password" label="Hasło" :rules="[ruleRequired]"></v-text-field>
                    <VAlert v-if="errorMsg" type="error" variant="tonal">{{ errorMsg }}</VAlert>
                </VCardText>    
                <VCardActions>
                    <v-btn class="mx-auto" color="primary" type="submit" variant="elevated" :loading="loading">Zaloguj</v-btn>
                </VCardActions>
            </VForm>
        </VCard>
    </VDialog>
</template>

<style lang="scss" scoped>

</style>

<script setup>
    const userStore = useUserStore();
    const { getErrorMessage } = useWebApiResponseParser();
    const { ruleRequired, ruleEmail } = useFormValidationRules();

    const show = computed(() => {
        return userStore.$state.isLoggedIn === false || userStore.$state.loading === true;
    });

    const errorMsg = ref("");
    const loading = ref(false);

    const viewModel = ref({
    email: '',
    password: ''
    });

    const submit = async (ev) => {
        const { valid } = await ev;
        if(valid){
            login();
        }
    }

    const login = () => {
        loading.value = true;
        errorMsg.value = "";

        useWebApiFetch("/User/Login", {
            method: 'POST',
            body: { ...viewModel.value },
            onResponseError: ({response}) => {
                errorMsg.value = getErrorMessage(response, {"InvalidLoginOrPassword": "Błędny login lub hasło"});
            }
        })
        .then((response) => {
            if(response.data.value){
                viewModel.value.password = '';
                userStore.loadLogedInUser();
            }
        })
        .finally(() => {
            loading.value = false;
        })
    };

</script>