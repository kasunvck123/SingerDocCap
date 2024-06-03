import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ImagecaptureComponent } from './imagecapture.component';

const routes: Routes = [
  {
    path: '',
    component: ImagecaptureComponent,
    data: {
      title: 'Image Capture'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ImageCaptureRoutingModule {}
