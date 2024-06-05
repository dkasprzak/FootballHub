<template>
    <VDialog :model-value="show" persistent width="400" scroll-strategy="none">
        <VCard class="py-4">
            <VCardTitle class="text-center">Logowanie</VCardTitle>
            <div v-if="userStore.$state.loading === true" class="pa-4 d-flex justify center">
                <VProgressCircular indeterminate></VProgressCircular>
            </div>
            <VForm v-else @submit.prevent="submit">
                <VCardText>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.email" label="Email"></v-text-field>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.password" type="password" label="Hasło"></v-text-field>
                </VCardText>    
                <VCardActions>
                    <v-btn class="mx-auto" color="primary" type="submit" variant="elevated">Zaloguj</v-btn>
                </VCardActions>
            </VForm>
        </VCard>
    </VDialog>
</template>

<style lang="scss" scoped>

</style>

<script setup>
    const userStore = useUserStore();

    const show = computed(() => {
        return userStore.$state.isLoggedIn === false || userStore.$state.loading === true;
    });

    const viewModel = ref({
    email: '',
    password: ''
    });

    const submit = () => {
        console.log(viewModel.value);
    }

</script>