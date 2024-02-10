import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';  
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { AppComponent } from './app.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HttpClientModule } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { EnglishWordsComponent } from './english-words/english-words.component';

@NgModule({
  declarations: [
    AppComponent,
    
  ],
  imports: [
    EnglishWordsComponent,
    MatIconModule,
    BrowserModule,
    MatDialogModule,        
    MatFormFieldModule,    
    MatToolbarModule,        
    BrowserAnimationsModule,
    FormsModule,
    MatInputModule,
    MatDividerModule,
    MatSelectModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,    
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatCardModule,
    HttpClientModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  exports : [EnglishWordsComponent],
  providers : [{ provide: MAT_DIALOG_DATA, useValue: {} },
      { provide: MatDialogRef, useValue: {} }],
  bootstrap: [AppComponent]
})
export class AppModule { }


