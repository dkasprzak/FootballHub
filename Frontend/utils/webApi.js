import { hash } from 'ohash' 

export const useWebApiFetch = function(request, opts){
    const config = useRuntimeConfig();
    const { getErrorMessage } = useWebApiResponseParser();
    const globalMessageStore = useGlobalMessageStore();

    return useFetch(request, {baseURL: config.public.BASE_URL,
        onRequest({request, options}){

        },
        onRequestError(context){

        },
        onResponse({request, response, options}){

        },
        onResponseError({ request, response, options }) {
            const message = getErrorMessage(response, {});
            globalMessageStore.showErrorMessage(message);
        },
        credentials: 'include',
        key: hash(['webapi-fetch', request, opts?.body, opts?.params, opts?.method, opts?.query]),
        ...opts});
}