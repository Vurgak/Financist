import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BookCardModel } from "@app/models/books/book-card.model";

@Component({
  selector: 'f-book-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './book-card.component.html'
})
export class BookCardComponent {
  @Input()
  book!: BookCardModel;
}
