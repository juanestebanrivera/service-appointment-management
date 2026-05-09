import { HttpInterceptorFn } from '@angular/common/http';
import { authInterceptor } from './auth-interceptor';

export * from './auth-interceptor';

export const coreInterceptors: HttpInterceptorFn[] = [authInterceptor];
