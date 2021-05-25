import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AddInventoryComponent } from '../add-inventory/add-inventory.component';
import { InventoryModel } from '../Inventory-Model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {

  inventoryItemsList = [];
  selectedRecord: any;
  rowClicked;
  isDisableButton = true;


  constructor(
    private http: HttpClient,
    private matDialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.getInventoryItems();
  }

  addInventoryItem(): void {
    const dialogRef = this.matDialog.open(AddInventoryComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getInventoryItems();
      console.log('The dialog was closed');
    });
  }

  getInventoryItems() {
    this.http.get<InventoryModel[]>(environment.url.inventory).subscribe(
      data => {
        this.inventoryItemsList = data;
      },
      error => {
        console.error(error);
      });
  }


  editInventoryItem(data) {
    this.selectedRecord = data;
    const dialogRef = this.matDialog.open(AddInventoryComponent, {
      width: '400px',
      data: {
        model: this.selectedRecord
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  deleteInventoryItem(data) {
    this.selectedRecord = data;
    const Id = data.id;
    this.http.delete(`${environment.url.inventory}?Id=${Id}`).subscribe(
      () => {
        this.snackBar.open('Deleted successfully','', {
          duration: 2000,
        });
        console.log('Item removed successfully');
        this.selectedRecord = null;
        this.getInventoryItems();
      },
      error => {
        console.error(error);
      });
  }

  openDialog(details) {
    const dialogRef = this.matDialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Are you sure want to delete?',
        buttonText: {
          ok: 'Yes',
          cancel: 'No'
        }
      }
    });

    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteInventoryItem(details)
        const a = document.createElement('a');
        a.click();
        a.remove();
      }
    });
  }


}
