import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { InventoryModel } from '../Inventory-Model';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from "@angular/material/snack-bar";


@Component({
  selector: 'app-add-inventory',
  templateUrl: './add-inventory.component.html',
  styleUrls: ['./add-inventory.component.css']
})
export class AddInventoryComponent implements OnInit {
  model: any = {};
  homeObject;
  isUpdateButton = false;

  constructor(private http: HttpClient,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) data,
    public dialogRef: MatDialogRef<AddInventoryComponent>,
  ) {
    if (data !== null) {
      this.model = data.model;
      this.isUpdateButton = true;
    }
  }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  saveInventoryItem() {
    const saveRequest: InventoryModel = {
      id: this.model.id,
      name: this.model.name,
      description: this.model.description,
      price: this.model.price,
      category: this.model.category
    };

    this.http.post(environment.url.inventory, saveRequest)
      .subscribe(response => {
        if (this.isUpdateButton) {
          this.snackBar.open('Updated successfully', '', {
            duration: 2000,
          });
        } else {
          this.snackBar.open('Added successfully', '', {
            duration: 2000,
          });
        }
        
        console.log(response);
        this.onNoClick();
      },
        error => {
          console.error(error);
        });
    this.model = {};
  }

  closeInventoryDialog() {
    this.dialogRef.close();
  }
}
