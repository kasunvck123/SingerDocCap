import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { ImagecaptureComponent } from './imagecapture.component';
import { TwoDigitDecimaNumberDirective } from '../../Common/TwoDigitDecimaNumberDirective';
import { ImageCaptureRoutingModule } from './imagecapture-routing.module';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CommonModule } from '@angular/common';
import { WebcamModule } from 'ngx-webcam';
import { ImageCaptureService } from '../../views/imagecapture/imageCapture.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CommonDirectiveModule } from '../../Common/common.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ImageCaptureRoutingModule,
    ChartsModule,
    BsDropdownModule, TabsModule,
    WebcamModule,
    NgSelectModule, 
    ButtonsModule.forRoot(), AlertModule.forRoot(), CommonDirectiveModule
  ],
  declarations: [ImagecaptureComponent],
  providers: [
    ImageCaptureService,
  ],
})
export class ImagecaptureModule { }
