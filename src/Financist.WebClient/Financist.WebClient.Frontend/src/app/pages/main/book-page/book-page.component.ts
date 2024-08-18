import { Component, Input, OnInit, inject, signal } from "@angular/core";
import { BookModel } from "@app/models/books/book.model";
import { BooksService } from "@app/services/books.service";

@Component({
  selector: "f-book-page",
  standalone: true,
  imports: [],
  templateUrl: "./book-page.component.html",
})
export class BookPageComponent implements OnInit {
  @Input()
  bookId: string | null = null;

  book: BookModel | null = null;

  private booksService = inject(BooksService);

  ngOnInit() {
    console.log("asdf", this.bookId);
    if (this.bookId == null)
      return;

    this.booksService.getBookById(this.bookId)
      .subscribe(book => this.book = book);
  }
}
