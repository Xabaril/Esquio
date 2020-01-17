import { container } from 'inversify-props';
import { AuditService } from './audit.service';
import { IAuditService } from './iaudit.service';

export default () => {
  container.addSingleton<IAuditService>(AuditService);
};
