export const useFileConverter = () => {
    return{
        base64ToBlob: (base64String) => {
            const byteCharacters = atob(base64String.split(',')[1]);
            const byteNumbers = new Array(byteCharacters.length);
            for(let i = 0; i < byteCharacters.length; i++){
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            
            const byteArray = new Uint8Array(byteNumbers);
            const mimeType = base64String.split(';')[0].split(':')[1];
            return new Blob([byteArray], { type: mimeType});
        },
        
        base64ToFile: (base64String, fileName) => {
            const blob = useFileConverter().base64ToBlob(base64String);
            return new File([blob], fileName, { type: blob.type });
        },

    };
};  
