import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InquiryComponent } from './inquiry.component';

const routes: Routes = [
  {
    path: '',
    component: InquiryComponent,
    data: {
      title: 'Inquiry'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ImageCaptureRoutingModule {}
