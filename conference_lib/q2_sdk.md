# Q2 SDK
========================
1. Login
```
# pass: test123
ssh sdk-shared-dev.q2devstack.com
# remember the port
echo $ANTILLES_SERVER_PORT
```
2. Notes for Tutorial
```
   0) using vpn so far.
   1) when downloading project.zip file, username and password should be same to the used for SSH;
   2) About DB settings, General username and pass(for the sandbox), SSH/SSL username and pass(your own, same to 1)
   3) Local/Project/Path should be the real path to your project.
   4) Don't need to create a new project following the Tutorial. The "local project for Q2 employee" means remote access for us.
   5) sometimes the updated template will not be pushed up. need to change it again to push up again.
```
3. Commands for Q2
```
   # install q2 sdk
   pip install q2_sdk==2.0.0a4
   # upgrade q2 sdk
   q2 upgrade -p sdk
   # create extention, then in the configuration/settings.py add your new extension
   q2 create_extension AccountDashboard
   # install form
   q2 install_form
   # add navigation menu
   q2 add_to_nav
   # run q2
   q2 run
   # install a library
   q2 add_dependency arrow
   # 
```
4. jinja2 template and default()
```
<div class="row header-row">
    <span>Host Account ID</span>
    <span>Account Name</span>
    <span>Account Balance</span>
</div>

{% for account in accounts %}
<div class="row account-list">
    <span>{{ account.account_num }}</span>
    <span>{{ account.account_name }}</span>
    <span>{{ account.account_balance }}</span>
</div>
{% endfor %}

<div class="row">
    <div class="form-group">
        <label for='input_1'>Account ID
            <span class="text-danger">*</span>
        </label>
        <input
                type='text'
                name='account_id'
                id='account_id'
                class="form-control required"
                required onkeyup="validate_input_fields()">
    </div>
</div>
# default()
async def default(self):
    account_models = []
    for account in self.account_list:
        account_models.append({
            'account_num': account.host_acct_id,
            'account_name': account.product_name,
            'account_balance': account.balance_to_display
        })

    template = self.get_template('index.html.jinja2', {
        'accounts': account_models
    })

    html = forms.Q2Form(
        "AccountDashboard",
        custom_template=template,
        routing_key="submit"
    )
    return html
```
5. HQ API
```
<div>
    <a href="#default">Back to Accounts</a>
</div>
<table class="table table-alt-shaded">
    <thead>
        <th>Date</th>
        <th>Amount</th>
        <th>Description</th>
    </thead>
    <tbody>
        {% for transaction in transactions %}
            <tr>
                <td>{{transaction.display_date}}</td>
                {% if transaction.is_credit %}
                    <td class="green">+{{transaction.display_amount}}</td>
                {% else %}
                    <td>{{transaction.display_amount}}</td>
                {% endif %}
                <td>{{transaction.display_description}}</td>
            </tr>
        {% endfor %}
    </tbody>
</table>
# submit()
from q2_sdk.hq.hq_api.q2_api import GetAccountHistoryById
import arrow
async def submit(self):
    params_obj = GetAccountHistoryById.ParamsObj(
        self.logger,
        self.form_fields['account_id'],
        ''
    )

    hq_response = await GetAccountHistoryById.execute(params_obj)

    transaction_models = []

    for transaction in hq_response.result_node.Data.AllHostTransactions.Transactions:
        date_string = str(transaction.PostDate)
        arrow_date_object = arrow.get(date_string, 'YYYY-MM-DD')
        display_date = arrow_date_object.format('MMMM DD, YYYY')
        # display_date = str(transaction.PostDate)
        display_amount = str(abs(transaction.TxnAmount))
        is_credit = (transaction.TxnAmount > 0)
        display_description = str(transaction.Description)

        transaction_models.append({
            'display_date': display_date,
            'display_amount': display_amount,
            'is_credit': is_credit,
            'display_description': display_description
        })

    template = self.get_template(
        'transaction_history.html.jinja2', {
            'header': "Transaction History",
            'transactions': transaction_models,
            'current_account_id': self.form_fields['account_id'],
        }
    )

    html = forms.Q2Form(
        "Transaction History",
        custom_template=template,
        hide_submit_button=True
    )

    return html
```
6. Q2 request
```
from q2_sdk.core import q2_requests
response = await q2_requests.get(self.logger, 'https://www.q2ebanking.com/')
# response
{
  "success":true,
  "terms":"https:\/\/currencylayer.com\/terms",
  "privacy":"https:\/\/currencylayer.com\/privacy",
  "source":"USD",
  "quotes":{
    "USDAED":3.672696,
    "USDAFN":71.150002,
    "USDALL":107.949997,
    "USDAMD":482.720001,
    "USDANG":1.790388,
    "USDAOA":237.675995,
    "USDARS":25.420509,
    "USDAUD":1.315697,
    "USDAWG":1.78,
    "USDAZN":1.699503,
    ...
  }
}
async def get_currency_rates(self):
    response = await q2_requests.get(self.logger, 'http://www.apilayer.net/api/live',
                                     params={
                                         'access_key': 'fc5715a78d9ad83078e64968eb282d83'
                                     })

    response_as_dict = response.json()

    try:
        quotes = response_as_dict['quotes']
        quotes_dictionary = {}

        # Reformat our quotes into a more display friendly format
        for quotes_key, quotes_value in quotes.items():
            new_key = quotes_key[3:]
            quotes_dictionary[new_key] = quotes_value

    except KeyError:
        quotes_dictionary = {}

    return quotes_dictionary
async def submit(self):
    rates_dictionary = await self.get_currency_rates()

    sorted_rates = sorted(rates_dictionary.keys())
    current_account_id = self.form_fields['account_id']
    selected_currency = self.form_fields.get('currency', 'USD')

    params_obj = GetAccountHistoryById.ParamsObj(
        self.logger,
        current_account_id,
        ''
    )

    hq_response = await GetAccountHistoryById.execute(params_obj)

    transaction_models = []
    for transaction in hq_response.result_node.Data.AllHostTransactions.Transactions:
        date_string = str(transaction.PostDate)

        arrow_date_object = arrow.get(date_string, 'YYYY-MM-DD')
        display_date = arrow_date_object.format('MMMM DD, YYYY')

        is_credit = (transaction.TxnAmount > 0)

        if selected_currency and selected_currency != 'USD':
            # multiply our USD amount by the retrieved exchange rate
            # return a string representation of the absolute value of the amount
            # in our new category rounded to 2 decimal places
            modified_amount = transaction.TxnAmount * rates_dictionary[selected_currency]
            display_amount = format(abs(modified_amount), '.2f')
        else:
            # if we are displaying USD, just return a string representation of
            # the absolute value of the amounts as before
            display_amount = str(abs(transaction.TxnAmount))

        display_description = str(transaction.Description)

        transaction_models.append({
            'display_date': display_date,
            'display_amount': display_amount,
            'is_credit': is_credit,
            'display_description': display_description
        })

    template = self.get_template(
        'transaction_history.html.jinja2',
        {
            'header': 'Transaction History',
            'transactions': transaction_models,
            'currency_options': sorted_rates,
            'current_account_id': self.form_fields['account_id'],
            'current_selected_currency': selected_currency
        }
    )

    html = forms.Q2Form(
        "Transaction History",
        custom_template=template,
        hide_submit_button=True
    )

    return html
# related jinja2
<div>
    <a href="#default">Back to Accounts</a>
</div>

<label>Currency</label>
<select id="select_currency" name="currency">
  {% for currency in currency_options %}
      {% if currency == current_selected_currency %}
        <option value="{{currency}}" selected>{{currency}}</option>
      {% else %}
        <option value="{{currency}}">{{currency}}</option>
      {% endif %}
  {% endfor %}
</select>

<input id="hidden_account_id_storage" name="account_id" value="{{current_account_id}}" type="hidden">

<div>
    <a href="#submit">Convert Currency</a>
</div>

<table class="table table-alt-shaded">
    <thead>
        <th>Date</th>
        <th>Amount</th>
        <th>Description</th>
    </thead>
    <tbody>
        {% for txn in transactions %}
            <tr>
                <td>{{txn.display_date}}</td>
                {% if not txn.is_credit %}
                    <td class="amount">{{txn.display_amount}} {{current_selected_currency}}</td>
                {% else %}
                    <td class="amount green">+{{txn.display_amount}} {{current_selected_currency}}</td>
                {% endif %}
                <td>{{txn.display_description}}</td>
            </tr>
        {% endfor %}
    </tbody>
</table>
# configuration/settings
OUTBOUND_WHITELIST = ['localhost', 'www.apilayer.net']
```
