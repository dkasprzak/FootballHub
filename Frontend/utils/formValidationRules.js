export const  useFormValidationRules = () => {
    return {
        ruleRequired: (v) => !!v || "To pole jest wymagane",
        ruleEmail: (value) => {
            const patern =
                /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return patern.test(value) || "Niepoprawny adres email";   
        },
        ruleMaxLen: (max) => {
            return (v) => (!v || v.length <= max) || `Przekroczono maksymalną długość: ${max}`;
        }
    };
};