import { inject, Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { BookCardModel } from "@app/models/books/book-card.model";
import { BookModel } from "@app/models/books/book.model";
import { NewBookModel } from "@app/models/books/new-book.model";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: "root",
})
export class BooksService {
  private httpClient = inject(HttpClient);

  addBook(book: NewBookModel): Observable<BookModel> {
    return this.httpClient.post<BookModel>("api/books", book);
  }

  getBooksByUserId(userId: string): Observable<BookCardModel[]> {
    return this.httpClient.get<BookCardModel[]>(`api/users/${userId}/books`);
  }

  getBookById(bookId: string): Observable<BookModel> {
    return this.httpClient.get<BookModel>(`api/books/${bookId}`);
  }
}
