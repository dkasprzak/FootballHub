<template>
    <VCard>
        <v-toolbar color="transprent">
            <template v-slot:prepend>
                <v-btn icon="mdi-chevron-left" to="/leagues"></v-btn>
            </template>

            <v-toolbar-title>
                {{ isAdd ? 'Dodaj ligę' : 'Edycja ligi' }}
            </v-toolbar-title>
        </v-toolbar>

        <VSkeletonLoader v-if="loading" type="paragraph, actions"></VSkeletonLoader>
        <VForm v-else @submit.prevent="submit" :disabled="saving">
            <div class="league-profile">
                <VCardText class="d-flex justify-center align-center">
                    <v-avatar size="150">
                        <v-img :src="imageUrl" alt="Logo"></v-img>
                    </v-avatar>
                </VCardText>
                <VCardText class="w-75">
                    <VTextField :rules="[ruleRequired, ruleMaxLen(200)]" variant="outlined" label="Nazwa ligi" v-model="viewModel.leagueName"/>
                    
                    <v-autocomplete :rules="[ruleRequired]" :items="countryStore.$state.countriesData" 
                    item-title="name" item-value="id" v-model="viewModel.countryId" variant="outlined" label="Kraj"></v-autocomplete>
                    
                    <v-file-input :rules="[ruleRequired]" variant="outlined" label="Wybierz zdjęcie" accept="image/*" 
                    @change="onFileChange($event)" v-model="viewModel.logo"></v-file-input>
                </VCardText>
            </div>

            <VCardText class="text-right">
                <VBtn prepend-icon="mdi-content-save" variant="flat" color="primary" type="submit" :loading="saving" 
                    :disabled="loading">Zapisz
                </VBtn>
            </VCardText>
        </VForm>
    </VCard>
</template>

<style lang="scss" scoped>
    .league-profile{
        display: flex;
    }
</style>

<script setup>
    const route  = useRoute();
    const router = useRouter();
    const { getErrorMessage } = useWebApiResponseParser();
    const globalMessageStore = useGlobalMessageStore();
    const countryStore = useCountryStore();
    const { ruleRequired, ruleMaxLen } = useFormValidationRules();
    const { base64ToFile } = useFileConverter();

    const saving = ref(false);
    const loading = ref(false);

    const imageUrl = ref(null);

    const isAdd = computed(() => {
        return route.params.id === 'add';
    });

    const viewModel = ref({
       leagueName: '',
       logo: '',
       countryId: '',
    });

    const getModel = ref({
        leagueName: '',
        logo: '',
        logoFileName: '',
        countryName: ''
    });

    const onFileChange = (file) => {
    const fileObject = file.target.files[0];
    if (fileObject) {
        const reader = new FileReader();
        reader.onload = (e) => {
            imageUrl.value = e.target.result;
        };
        reader.readAsDataURL(fileObject);
        viewModel.value.logo = fileObject;
    }
    };
    const loadData = () => {
        loading.value = true;
        let id = route.params.id;

        useWebApiFetch(`/League/GetLeagueDetails/${id}`)
        .then(({data, error}) => {
            if(data.value){
                getModel.value = data.value;
                viewModel.value.leagueName = getModel.value.leagueName;
                viewModel.value.countryId=  countryStore.countriesData.find(country => country.name === getModel.value.countryName)?.id;
                imageUrl.value = getModel.value.logo;
                viewModel.value.logo = base64ToFile(getModel.value.logo, getModel.value.logoFileName);
            } else if(error.value){
                globalMessageStore.showErrorMessage("Błąd pobierania danych");
            }
        }).finally(() => {
            loading.value = false;
        });
    };

    const createOrUpdate = () => {
        saving.value = true;

        const formData = new FormData();
        if (!isAdd.value) {
            formData.append('id', route.params.id);
        }
        formData.append('leagueName', viewModel.value.leagueName);
        formData.append('countryId', viewModel.value.countryId);
        formData.append('logo', viewModel.value.logo);

        useWebApiFetch('/League/CreateOrUpdateLeague', {
            method: 'POST',
            body: formData,
            watch: false,
            onResponseError: ({ response }) => {
                let message = getErrorMessage(response, {});
                globalMessageStore.showErrorMessage(message);
            }
        })
        .then((response) => {
            if(response.data.value){
                globalMessageStore.showSuccessMessage('Zapisano zmiany.');
                router.push({path: '/leagues'});
            }
        })
        .finally(() => {
            saving.value = false;
        });
    }

    const submit = async (ev) => {
        const { valid } = await ev;
        if(valid){
            createOrUpdate();
        }
    }

    onMounted(() => {
        if(!isAdd.value){
            loadData();
        }
        countryStore.loadCountries();
    });
</script>
