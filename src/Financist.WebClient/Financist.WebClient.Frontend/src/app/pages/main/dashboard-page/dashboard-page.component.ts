import { Component } from '@angular/core';
import { BookListComponent } from "@app/components/books/book-list/book-list.component";

@Component({
  selector: 'f-dashboard-page',
  standalone: true,
  imports: [BookListComponent],
  templateUrl: './dashboard-page.component.html',
})
export class DashboardPageComponent {
}
