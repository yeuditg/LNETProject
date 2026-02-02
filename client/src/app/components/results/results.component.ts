import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/app.service';
import { Question } from '../../types/question';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css'],
  imports: [CommonModule]
})
export class ResultsComponent implements OnInit {
  results: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getQuestions().subscribe(questions => {
      questions.forEach((q: Question) => {
        this.api.getResults(q.id).subscribe(res => this.results.push({ question: q.text, answers: res }));
      });
    });
  }
}
