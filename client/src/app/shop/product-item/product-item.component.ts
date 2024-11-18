import { Component, Input } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product?: Product;

  formatPrice(price: number): string {
    return price.toLocaleString('fa-IR'); // استفاده از فرمت فارسی
}

constructor(private basketService: BasketService){}

addItemToBasket(){
  this.product && this.basketService.addItemToBasket(this.product);
}


}
