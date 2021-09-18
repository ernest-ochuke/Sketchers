import { Component, Input, OnInit } from '@angular/core';
import { CdkStepper } from '@angular/cdk/stepper';
import { Observable } from 'rxjs';
import { IBasket } from 'src/app/shared/models/basket';
import { BasketService } from 'src/app/basket/basket.service';
import { ToastrService } from 'ngx-toastr'
@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper: CdkStepper;
  basket$: Observable<IBasket>;
  constructor(private basketService: BasketService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe((response: any) => {
     this.toastr.success('payment intent created')
      this.appStepper.next();
    }, error => {
      console.log("review and payment intent error " + error)
      this.toastr.error(error.message);
    })
  }

}
