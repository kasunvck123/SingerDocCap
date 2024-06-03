import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { InquiryComponent } from './inquiry.component';
import { TwoDigitDecimaNumberDirective } from '../../Common/TwoDigitDecimaNumberDirective';
import { ImageCaptureRoutingModule } from './inquiry-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { DatePipe,CommonModule } from '@angular/common';
import { WebcamModule } from 'ngx-webcam';
import { InquiryService } from '../inquiry/inquiry.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CommonDirectiveModule } from '../../Common/common.module';
import { DataTablesModule } from "angular-datatables";
import { ImageCaptureService } from '../../views/imagecapture/imageCapture.service';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ImageCaptureRoutingModule,
    ChartsModule,
    BsDropdownModule, TabsModule,
    WebcamModule,  
    NgSelectModule,
   
    ButtonsModule.forRoot(), AlertModule.forRoot(), CommonDirectiveModule, DataTablesModule
  ],
  declarations: [InquiryComponent],
  providers: [
    InquiryService, DatePipe, ImageCaptureService
  ],
})
export class InquiryModule { }
