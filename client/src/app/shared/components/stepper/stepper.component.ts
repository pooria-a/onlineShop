import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  providers: [{provide: CdkStepper, useExisting:StepperComponent}]
})
export class StepperComponent extends CdkStepper implements OnInit {
  
  ngOnInit(): void {
    this.linear = this.linearModeSelected;
  }
  @Input() linearModeSelected = true;

  onClick(index: number ){
    this.selectedIndex = index;
  }

}
