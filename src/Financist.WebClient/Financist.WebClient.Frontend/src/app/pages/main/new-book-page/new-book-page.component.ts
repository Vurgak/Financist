import { Component, inject } from "@angular/core";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { BooksService } from "@app/services/books.service";
import { NewBookModel } from "@app/models/books/new-book.model";

@Component({
  selector: "f-new-book-page",
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: "./new-book-page.component.html",
})
export class NewBookPageComponent {
  form = new FormGroup({
    name: new FormControl("", [Validators.required, Validators.minLength(3)]),
  });

  private router = inject(Router)
  private booksService = inject(BooksService)

  onSubmit() {
    console.log(this.form.getRawValue())
    const book = this.form.getRawValue() as NewBookModel;
    this.booksService.addBook(book).subscribe({
      next: book => this.router.navigate(["/book", book.bookId]),
    });
  }
}
