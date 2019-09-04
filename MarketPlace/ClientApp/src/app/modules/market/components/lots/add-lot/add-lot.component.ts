import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MarketService } from 'src/app/shared/services/market.service';

@Component({
  selector: 'app-add-lot',
  templateUrl: './add-lot.component.html',
  styleUrls: ['./add-lot.component.css']
})
export class AddLotComponent implements OnInit {

  lotForm: FormGroup;
  error: string;
  id = null;
  constructor(private formBuilder: FormBuilder, private mS: MarketService, private route: ActivatedRoute) {
  }


  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    console.log(this.id);
    this.lotForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(64)]],
      description: ['', [Validators.required]],
      price: ['', [Validators.required, Validators.min(10)]]
    });
  }
  onSubmit() {
    const data = this.lotForm.value;
    console.log(data.email);
    this.mS.newLot(this.id, data.name, data.description, data.price);
  }

}
