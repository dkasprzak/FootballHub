<template>
    <VCard>
        <v-toolbar color="transparent">
            <v-toolbar-title>
                Ligi piłkarskie
            </v-toolbar-title>

            <template v-slot:append>
                <div class="ml-2">
                    <v-btn color="primary" variant="flat" prepend-icon="mdi-plus" to="/leagues/add">Dodaj</v-btn>
                </div>
            </template>
        </v-toolbar>

        <v-data-table :loading="loading" :items="items" :headers="headers" items-per-page-text="Liczba wierszy na stronę"
            :items-per-page-options="[10, 20, 50]" page-text="{0}-{1} z {2}" no-data-text="Nie dodano żadnej ligi" loading-text="Trwa wczytywanie danych">
            
            <template v-slot:item.createDate="{ value }">
                {{ dayjs(value).format("DD.MM.YYYY") }}
            </template>

            <template v-slot:item.action="{ item }">
                <div class="text-no-wrap">
                    <VBtn icon="mdi-delete" tile="Usuń" variant="flat" :loading="item.deleting" @click="deleteLeague(item)"></VBtn>
                    <VBtn icon="mdi-pencil" title="Edytuj" variant="flat" :to="`/leagues/${item.id}`"></VBtn>
                </div>
            </template>

        </v-data-table>
        <ConfirmDialog ref="confirmDialog"/>
    </VCard>
</template>

<script setup>
    const { getErrorMessage } = useWebApiResponseParser();
    const globalMessageStore = useGlobalMessageStore();
    const dayjs = useDayjs();
    const loading = ref(false);
    const items = ref([]);

    const confirmDialog = ref(null);


    const headers = ref([
       { title: 'Nazwa', value: 'leagueName' },
       { title: 'Kraj', value: 'countryName' },
       { title: 'Kraj skrót', value: 'countryShortName' },
       { title: 'Data utworzenia', value: 'createDate' },
       { title: '', value: 'action', align: 'end' },
    ]);

    const loadData = () => {
        loading.value = true;
        
        useWebApiFetch('/League/GetLeagues')
        .then(({data, error}) => {
            if(data.value){
                items.value = data.value.leagues;
            } else if(error.value){
                items.value = [];
            }
        })
        .finally(() => {
            loading.value = false;
        });
    };

    const deleteLeague = (item) => {
        confirmDialog.value.show({
            title: 'Potwierdzenie usunięcia',
            text: 'Czy na pewno chcesz usunąć ligę?',
            confirmBtnText: 'Usuń',
            confirmBtnColor: 'error'
        }).then((confirm) => {
            if(confirm){
                item.deleting = true;
                useWebApiFetch('/League/DeleteLeague', {
                    method: 'POST',
                    body: { id: item.id },
                    watch: false,
                    onResponseError: ({ response }) => {
                        let message = getErrorMessage(response, {});
                        globalMessageStore.showErrorMessage(message);
                    } 
                })
                .then((response) => {
                    if(response.data.value){
                        globalMessageStore.showSuccessMessage('Liga została usunięta.');
                        let indexToDel = items.value.findIndex(i => i.id === item.id);
                        if(indexToDel > -1){
                            items.value.splice(indexToDel, 1);
                        }
                    }
                })
                .finally(() => {
                    item.deleting = false;
                });
            }
        })
    }

    onMounted(() => {
        loadData();
    })
</script>