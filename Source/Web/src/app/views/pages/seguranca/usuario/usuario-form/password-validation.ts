import {AbstractControl} from '@angular/forms';
export class PasswordValidation {

    static MatchPassword(AC: AbstractControl) {
       let password = AC.get('TX_SENHA').value; // to get value in input tag
       let confirmPassword = AC.get('CONFIRMA_SENHA').value; // to get value in input tag
        if(password != confirmPassword) {
            console.log('false');
            AC.get('CONFIRMA_SENHA').setErrors( {MatchPassword: true} )
        } else {
            console.log('true');
            return null
        }
    }
}