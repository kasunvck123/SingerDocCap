import { NgModule } from '@angular/core';
import { TwoDigitDecimaNumberDirective } from '../../app/Common/TwoDigitDecimaNumberDirective';


@NgModule({
  declarations: [TwoDigitDecimaNumberDirective],
  exports: [TwoDigitDecimaNumberDirective]
})
export class CommonDirectiveModule { }
