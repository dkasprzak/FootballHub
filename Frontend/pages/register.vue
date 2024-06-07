<template>
    <div class="d-flex align-center justify-center fill-height"> 
        <VCard width="600">
            <VCardTitle>Zarejestruj się</VCardTitle>
            <VForm @submit.prevent="submit" :disabled="loading">
                <VCardText>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.email" label="Email" :rules="[ruleRequired, ruleEmail]"></v-text-field>
                    <v-text-field class="mb-4" variant="outlined" v-model="viewModel.password" type="password" label="Hasło" :rules="[ruleRequired]"></v-text-field>
                    <v-text-field class="mb-4" variant="outlined" type="password" label="Powtórz hasło" :rules="[ruleRequired, rules.samePassword]"></v-text-field>
                    <VAlert v-if="errorMsg" type="error" variant="tonal">{{ errorMsg }}</VAlert>
                </VCardText>    
                <VCardActions>
                    <v-btn class="mx-auto" color="primary" type="submit" variant="elevated" :loading="loading">Utwórz konto</v-btn>
                </VCardActions>
            </VForm>
        </VCard>
    </div>
</template>

<style lang="scss" scoped>

</style>

<script setup>
import { registerRuntimeCompiler } from 'vue';
import { errorMessages } from 'vue/compiler-sfc';

    const userStore = useUserStore();
    const { getErrorMessage } = useWebApiResponseParser();
    const { ruleRequired, ruleEmail } = useFormValidationRules();
    const router = useRouter(); // przejście na inną stronę
    const globalMessageStore = useGlobalMessageStore();

    definePageMeta({
        layout: "no-auth",
    });

    const errorMsg = ref("");
    const loading = ref(false);


    const viewModel = ref({
       email: '',
       password: '' 
    });

    const submit = async (ev) => {
        const valid = await ev;
        if(valid){
            register();
        }
    }

    const register = () => {
        loading.value = true;
        errorMsg.value = "";
        
        useWebApiFetch('/User/CreateUserWithAccount', {
            method: 'POST',
            body: { ...viewModel.value },
            onResponseError: ({response}) => {
                errorMsg.value = getErrorMessage(response, {"AccountWithThisEmailAlreadyExists": "Konto z tym adresem email już istnieje"});
            }
        })
        .then((response) => {
            if(response.data.value){
                globalMessageStore.showSuccessMessage('Twoje konto zostało utworzone. Zalogowano do aplikacji.');
                router.push({ path: '/' });
            }
        })
        .finally(() => {
            loading.value = false;
        });
    }

    const rules = {
        samePassword: (v) => v === viewModel.value.password || 'Hasła różnią się od siebie',
    }



</script>