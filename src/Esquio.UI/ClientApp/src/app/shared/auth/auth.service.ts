import { injectable } from 'inversify';
import Oidc from 'oidc-client';
import { User } from './user.model';
import { settings } from '~/core';
import { IAuthService } from './iauth.service';

// Configure logs
Oidc.Log.logger = window.console;
Oidc.Log.level = Oidc.Log.ERROR;

@injectable()
export class AuthService implements IAuthService {
  private manager: Oidc.UserManager;
  public user: User;
  private config: Oidc.UserManagerSettings = null;

  public init(): void {
    this.config = {
      authority: settings.Authority,
      client_id: 'web',
      redirect_uri: window.location.protocol + '//' + window.location.host + '/callback',
      post_logout_redirect_uri: window.location.protocol + '//' + window.location.host,

      // these two will be done dynamically from the buttons clicked, but are
      // needed if you want to use the silent_renew
      response_type: 'id_token token',
      scope: 'openid profile ' + settings.Audience,

      // this will toggle if profile endpoint is used
      loadUserInfo: true,

      // silent renew will get a new access_token via an iframe
      // just prior to the old access_token expiring (60 seconds prior)
      silent_redirect_uri: window.location.protocol + '//' + window.location.host + '/silent',

      // will revoke (reference) access tokens at logout time
      revokeAccessTokenOnSignout: true,

      // this will allow all the OIDC protocol claims to be visible in the window. normally a client app
      // wouldn't care about them or want them taking up space
      filterProtocolClaims: false,

      automaticSilentRenew: true,
      accessTokenExpiringNotificationTime: 3600 - (3600 / 0.15)
    };

    this.manager = new Oidc.UserManager(this.config);
  }

  public login(): Promise<void | Oidc.User> {
    return this.manager.signinRedirect({ scope: this.config.scope, response_type: this.config.response_type })
      .catch(error => {
        console.log('authService.login error: ', error);
      });
  }

  public logout(): Promise<void | Oidc.User> {
    return this.manager.signoutRedirect()
      .catch(error => {
        console.log('authService.logout error: ', error);
      });
  }

  public callback(): Promise<void | Oidc.User> {
    return this.manager.signinRedirectCallback().catch(error => {
      console.log('authService.callback error: ', error);
    });
  }

  public silentCallback(): Promise<void | Oidc.User> {
    return this.manager.signinSilentCallback().catch(error => {
      console.log('silent.callback error: ', error);
    });
  }

  public silent(): Promise<void | Oidc.User> {
    return this.manager.signinSilent()
      .catch(error => {
        console.log('authService.silent error: ', error);
        this.logout();
      });
  }

  public getUser(): Promise<void | Oidc.User> {
    return this.manager.getUser().catch(error => {
      console.log('authService.getUser error: ', error);
    });
  }

  public handleEvents(): void {
    this.manager.events.addUserLoaded(user => {
      console.log('#response', { message: 'User loaded' });
      this.getUser();
    });

    this.manager.events.addUserUnloaded(() => {
      console.log('#response', { message: 'User logged out locally' });
      this.getUser();
    });

    this.manager.events.addAccessTokenExpiring(() => {
      console.log('#response', { message: 'Access token expiring...' });
    });

    this.manager.events.addSilentRenewError(err => {
      console.log('#response', { message: 'Silent renew error: ' + err.message });
    });

    this.manager.events.addUserSignedOut(() => {
      console.log('#response', { message: 'User signed out of OP' });
    });
  }
}
