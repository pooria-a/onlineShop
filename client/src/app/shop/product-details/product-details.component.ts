import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { Breadcrumb } from 'xng-breadcrumb/lib/types/breadcrumb';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
product?: Product;
quantity = 1;
quantityInBasket = 0;

  constructor(private shopServis: ShopService
    , private activatedRoute: ActivatedRoute,
    private bcService:BreadcrumbService, private basketService: BasketService){
      this.bcService.set('@productDetails',' ')
    }
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    const id=this.activatedRoute.snapshot.paramMap.get('id');
    if (id) this.shopServis.getProduct(+id).subscribe({
      next: product => {
        this.product = product;
        this.bcService.set('@productDetails',product.name)
        this.basketService.basketSource$.pipe(take(1)).subscribe ({
          next:basket =>{

            const item = basket?.items.find(x => x.id === +id)
            if (item){
              this.quantity = item.quantity;
              this.quantityInBasket = item.quantity;
            }
          }
        })
      },
      error: error => console.log(error)
    })
  }
  incrementQuantity(){
    this.quantity++;

  }

  decrementQuantity(){
    this.quantity--;
  }
  updateBasket(){
    if(this.product){
      if(this.quantity > this.quantityInBasket){
        const itemToAdd = this.quantity - this.quantityInBasket;
        this.quantityInBasket += itemToAdd;
        this.basketService.addItemToBasket(this.product ,itemToAdd )
      } else{
        const itemsToRemove = this.quantityInBasket - this.quantity;
        this.quantityInBasket -= itemsToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemsToRemove);
      }
    }
  }

  get buttonText(){

    return this.quantityInBasket === 0 ? 'اضافه به سبد ':'بروزرسانی سبد خرید';
  }
}

