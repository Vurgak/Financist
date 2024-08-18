import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BookCardComponent } from "@app/components/books/book-card/book-card.component";
import { BookCardModel } from "@app/models/books/book-card.model";
import { BooksService } from "@app/services/books.service";
import { AuthenticationService } from "@app/services/authentication.service";

@Component({
  selector: 'f-book-list',
  standalone: true,
  imports: [RouterLink, BookCardComponent],
  templateUrl: './book-list.component.html'
})
export class BookListComponent implements OnInit {
  books: BookCardModel[] = [];

  private authenticationService = inject(AuthenticationService);
  private booksService = inject(BooksService);

  ngOnInit(): void {
    this.booksService.getBooksByUserId(this.authenticationService.userId)
      .subscribe({
        next: books => this.books = books }
      );
  }
}
