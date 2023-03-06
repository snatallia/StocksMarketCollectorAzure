import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Stocks Data Collector title';
  stocks: any;
  cols: any[];

  /**
   *
   */
   constructor(private http: HttpClient){
    this.cols = [];
   }
  ngOnInit(): void {
    this.getStocks();
    this.cols = [
      { field: 'symbol', header: 'Symbol' },
      { field: 'description', header: 'Description' }
    ];


  }

  getStocks(){
    this.http.get('https://api20221206123553.azurewebsites.net/api/stocks').subscribe({
      next: response => this.stocks = response,
      error: error => console.log(error),
      complete: () => console.log('Request is ')
    });
  }


}
