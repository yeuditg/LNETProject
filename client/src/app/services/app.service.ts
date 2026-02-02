import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Question } from '../types/question';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7237/api/questions';

  constructor(private http: HttpClient) {}

  getQuestions(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }

  createQuestion(data: Partial<Question>): Observable<any> {
    return this.http.post(`${this.baseUrl}`, data);
  }

  vote(questionId: number, answerId: number): Observable<any> {
   
    var l= this.http.post(`${this.baseUrl}/vote`, {
      questionId: questionId,
      answerId: answerId
    });
    console.log(l , "gfgdss");
    return l;
    
  }

  getResults(questionId: number): Observable<any> {
 return this.http.get(`${this.baseUrl}/${questionId}`);

  }
}
