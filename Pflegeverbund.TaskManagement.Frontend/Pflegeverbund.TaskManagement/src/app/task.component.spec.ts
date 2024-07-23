import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TaskComponent } from './task.component';

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [RouterTestingModule],
    declarations: [TaskComponent]
  }));

  it('should create the app', () => {
    const fixture = TestBed.createComponent(TaskComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'TaskApp'`, () => {
    const fixture = TestBed.createComponent(TaskComponent);
    const app = fixture.componentInstance;
    //expect(app).toEqual('TaskApp');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(TaskComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.content span')?.textContent).toContain('TaskApp app is running!');
  });
});
