import { Component, Input } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { CurrencyPipe } from '@angular/common';

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


}
