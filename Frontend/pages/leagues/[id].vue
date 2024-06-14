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
            <VCardText>
                <VTextField :rules="[ruleRequired, ruleMaxLen(200)]" variant="outlined" label="Nazwa ligi" v-model="viewModel.leagueName"/>
                
                <v-autocomplete :rules="[ruleRequired]" :items="countryStore.$state.countriesData" 
                item-title="name" item-value="id" v-model="viewModel.countryId" variant="outlined" label="Kraj"></v-autocomplete>
                
                <v-file-input :rules="[ruleRequired]" variant="outlined" label="Wybierz zdjęcie" accept="image/*" 
                @change="onFileChange" v-model="viewModel.logo">
            </v-file-input>
            </VCardText>
            <VCardText class="text-right">
                <VBtn prepend-icon="mdi-content-save" variant="flat" color="primary" type="submit" :loading="saving" 
                    :disabled="loading">Zapisz
                </VBtn>
            </VCardText>
        
        </VForm>
    </VCard>
</template>

<script setup>

    const route  = useRoute();
    const router = useRouter();
    const { getErrorMessage } = useWebApiResponseParser();
    const globalMessageStore = useGlobalMessageStore();
    const countryStore = useCountryStore();
    const { ruleRequired, ruleMaxLen } = useFormValidationRules();

    const saving = ref(false);
    const loading = ref(false);

    const imageUrl = ref(null);
    const fileData = ref(null);

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
        logoUrl: '',
        countryName: ''
    });

    const onFileChange = (files) => {
    const file = files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            fileData.value = e.target.result;
            imageUrl.value = URL.createObjectURL(file);
        };
        reader.readAsArrayBuffer(file);
    }
    };

    const loadData = () => {
        loading.value = true;
        let id = route.params.id

        useWebApiFetch(`/League/GetLeagueDetails/${id}`)
        .then(({data, error}) => {
            if(data.value){
                getModel.value = data.value;
                viewModel.value.leagueName = getModel.value.leagueName;
                viewModel.value.countryId=  countryStore.countriesData.find(country => country.name === getModel.value.countryName)?.id;
            } else if(error.value){
                globalMessageStore.showErrorMessage("Błąd pobierania danych");
            }
        }).finally(() => {
            loading.value = false;
        });
    };

    const create = () => {
        saving.value = true;

        const formData = new FormData();
        formData.append('leagueName', viewModel.value.leagueName);
        formData.append('countryId', viewModel.value.countryId);
        formData.append('logo', viewModel.value.logo);

        useWebApiFetch('/League/CreateLeague', {
            method: 'POST',
            body: formData,
            watch: false,
            onResponseError: ({ response }) => {
                console.log(...viewModel.value)
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
            if(isAdd.value){
                create();
            } else{
                // edit();
            }
        }
    }


    onMounted(() => {
        if(!isAdd.value){
            loadData();
        }
        countryStore.loadCountries();
    });
</script>